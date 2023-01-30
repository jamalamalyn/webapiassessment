using Microsoft.EntityFrameworkCore;
using Xunit;
using PwC.ClientAPI.Domain;
using PwC.ClientAPI.Repository;
using PwC.ClientAPI.Controllers;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PwC.ClientAPI.DataMapping;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System;
using PwC.ClientAPI.Domain.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PcW.ClientAPI.Test
{
    public class ClientApiTests : IDisposable
    {
        private DbContextOptions<DataContext> dbContextOptions;
        private ClientRepository clientRepository;

        public ClientApiTests()
        {
            // Use in memory database for testing only.
            dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "ClientDatabase")
                .Options;

            clientRepository = CreateRepository();
        }

        [Fact]
        public void GetAllClientsTest_Success()
        {
            var controller = CreateClientController();

            var response = controller.GetAllClients();
            var resultObject = response.Result as OkObjectResult;

            var result = GetObjectResultContent(response);

            Assert.True(resultObject.StatusCode == 200);
            Assert.Equal(4, result.Clients.Count());
        }

        [Fact]
        public void GetClientsTest_Success()
        {
            var controller = CreateClientController();

            List<int> ids = new List<int>()
        {
            1,3,4
        };

            var response = controller.GetClients(ids);
            var resultObject = response.Result as OkObjectResult;

            var result = GetObjectResultContent(response);

            var matchedCount = result.Clients.Where(x => ids.Contains(x.Id)).ToList().Count;

            Assert.True(resultObject.StatusCode == 200);
            Assert.Equal(3, matchedCount);
        }

        [Fact]
        public void GetClientTest_Success()
        {
            var controller = CreateClientController();

            var response = controller.GetClient(2);
            var resultObject = response.Result as OkObjectResult;

            var result = GetObjectResultContent(response);

            Assert.True(resultObject.StatusCode == 200);
            Assert.Equal(2, result.Id);
        }

        [Fact]
        public void GetClientTest_IdNotExsits_Failure()
        {
            var controller = CreateClientController();

            var response = controller.GetClient(5);
            var result = response.Result as NotFoundResult;

            Assert.True(result.StatusCode == 404);
        }

        [Fact]
        public void AddClientTest_Success()
        {
            var controller = CreateClientController();

            var client = new ClientRequestObject()
            {
                Name = "Luna Lynch",
                EmailAddress = "test5@test.com",
                RegisteredDateTime = new DateTime(2020, 8, 22)
            };

            var response = controller.AddClient(client);
            var resultObject = response as OkResult;

            var result = clientRepository.Get(5);

            Assert.True(resultObject.StatusCode == 200);
            Assert.NotNull(result);
        }

        [Fact]
        public void AddClientsTest_EmptyInputFailure()
        {
            var controller = CreateClientController();

            var clients = new List<ClientRequestObject>() { };

            ClientRequest request = new ClientRequest()
            {
                Clients = clients
            };

            var response = controller.AddClients(request);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void AddClientsTest_NullListInputFailure()
        {
            var controller = CreateClientController();

            ClientRequest request = new ClientRequest()
            {
                Clients = null
            };

            var response = controller.AddClients(request);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void AddClientsTest_NullInputFailure()
        {
            var controller = CreateClientController();

            ClientRequest request = null;

            var response = controller.AddClients(request);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void UpdateClientTest_NullInputFailure()
        {
            var controller = CreateClientController();

            ClientRequestObject client = null;
            int id = 2;

            var response = controller.UpdateClient(id, client);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void UpdateClientsTest_Success()
        {
            var controller = CreateClientController();

            var clients = new List<Client>()
        {
            new Client(){
                Id = 2,
                Name = "Annette Gilpin"
            },
            new Client(){
                Id = 4,
                RegisteredDateTime = new DateTime(2020, 8, 30)
            },
            new Client(){
                Id = 3,
                EmailAddress = "nathan@test.com"
            }
        };

            var response = controller.UpdateClients(clients);
            var resultObject = response as OkResult;

            List<int> ids = new List<int>() { 2, 4, 3 };

            var updatedClients = clientRepository.GetRange(ids).ToList();

            Assert.True(resultObject.StatusCode == 200);
            Assert.NotEmpty(updatedClients);
            Assert.Equal("Annette Gilpin", updatedClients[0].Name);
            Assert.Equal("nathan@test.com", updatedClients[1].EmailAddress);
            Assert.Equal(new DateTime(2020, 8, 30), updatedClients[2].RegisteredDateTime);
        }

        [Fact]
        public void UpdateClientsTest_EmptyInputFailure()
        {
            var controller = CreateClientController();

            var clients = new List<Client>() { };

            var response = controller.UpdateClients(clients);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void UpdateClientsTest_NullInputFailure()
        {
            var controller = CreateClientController();

            List<Client> clients = null;

            var response = controller.UpdateClients(clients);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void UpdateClientsTest_NoIdFailure()
        {
            var controller = CreateClientController();

            var clients = new List<Client>()
        {
            new Client(){
                Name = "Annette Gilpin"
            },
            new Client(){
                RegisteredDateTime = new DateTime(2020, 8, 30)
            },
            new Client(){
                EmailAddress = "nathan@test.com"
            }
        };

            var response = controller.UpdateClients(clients);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }


        [Fact]
        public void DeleteClientTest_Success()
        {
            var controller = CreateClientController();

            int id = 2;

            var response = controller.DeleteClient(id);
            var resultObject = response as OkResult;

            var updatedClient = clientRepository.Get(id);

            Assert.True(resultObject.StatusCode == 200);
            Assert.Null(updatedClient);
        }

        [Fact]
        public void DeleteClientTest_IdNotExistFailure()
        {
            var controller = CreateClientController();

            int id = 5;

            var response = controller.DeleteClient(id);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void DeleteClientTest_NullIdFailure()
        {
            var controller = CreateClientController();

            int? id = null;

            var response = controller.DeleteClient(id);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void DeleteClientTests_Success()
        {
            var controller = CreateClientController();

            List<int> ids = new List<int>() { 2, 4 };

            var response = controller.DeleteClients(ids);
            var resultObject = response as OkResult;

            var removedClients = clientRepository.GetRange(ids);

            Assert.True(resultObject.StatusCode == 200);
            Assert.Empty(removedClients);
        }

        [Fact]
        public void DeleteClientTests_EmptyInputFailure()
        {
            var controller = CreateClientController();

            List<int> ids = new List<int>() { };

            var response = controller.DeleteClients(ids);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void DeleteClientTests_NullInputFailure()
        {
            var controller = CreateClientController();

            List<int> ids = null;

            var response = controller.DeleteClients(ids);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        [Fact]
        public void DeleteClientTests_IdsNotExistFailure()
        {
            var controller = CreateClientController();

            List<int> ids = new List<int>() { 5, 6 };

            var response = controller.DeleteClients(ids);
            var resultObject = response as BadRequestResult;

            Assert.True(resultObject.StatusCode == 400);
        }

        private ClientController CreateClientController()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = mapperConfig.CreateMapper();

            ILogger<ClientController> logger = new Logger<ClientController>(new NullLoggerFactory());

            return new ClientController(logger, clientRepository, mapper);
        }

        private void PopulateClients(DataContext context)
        {
            List<Client> clients = new List<Client>()
        {
            new Client()
            {
                Name = "Brendan Lynch",
                EmailAddress = "test1@test.com",
                RegisteredDateTime = new DateTime(2020, 8, 2)
            },
            new Client()
            {
                Name = "Annette Lynch",
                EmailAddress = "test2@test.com",
                RegisteredDateTime = new DateTime(2020, 8, 6)
            },
            new Client()
            {
                Name = "Nathan Clay",
                EmailAddress = "test3@test.com",
                RegisteredDateTime = new DateTime(2020, 8, 6)
            },
            new Client()
            {
                Name = "Bob Lynch",
                EmailAddress = "test4@test.com",
                RegisteredDateTime = new DateTime(2020, 8, 20)
            },
        };

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }


        private ClientRepository CreateRepository()
        {
            DataContext context = new DataContext(dbContextOptions);
            PopulateClients(context);
            return new ClientRepository(context);
        }

        private static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }

        public void Dispose()
        {
            clientRepository.DeleteDatabase();
            clientRepository.Dispose();
        }
    }
}
