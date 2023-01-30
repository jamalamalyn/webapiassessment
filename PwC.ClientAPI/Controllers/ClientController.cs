using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PwC.ClientAPI.Domain.Interfaces;
using PwC.ClientAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PwC.ClientAPI.Controllers
{
    [Route("v1/client")]
    public class ClientController
    {
        private IClientRepository _clientRepository;
        private ILogger _logger;
        private IMapper _mapper;

        public ClientController(ILogger logger, IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [Route("client/{id}")]
        [HttpGet]
        public ActionResult<ClientResponseObject> GetClient(int? id)
        {
            if (id != null && id > 0)
            {
                try
                {
                    var client = _clientRepository.Get(id);

                    if (client != null)
                    {
                        ClientResponseObject clientResponse = _mapper.Map<ClientResponseObject>(client);
                        return new OkObjectResult(clientResponse);
                    }
                    else
                    {
                        return new NotFoundResult();
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [Route("clients")]
        [HttpGet]
        public ActionResult<ClientResponse> GetClients(IEnumerable<int> ids)
        {
            try
            {
                IEnumerable<Client> clients;

                if (ids.Any())
                {
                    clients = _clientRepository.GetRange(ids);

                    if (clients.Any())
                    {
                        var clientResponse = _mapper.Map<IEnumerable<ClientResponseObject>>(clients);

                        return new OkObjectResult(new ClientResponse()
                        {
                            Clients = clientResponse
                        });
                    }
                    else
                    {
                        return new NotFoundResult();
                    }
                }
                else
                { 
                    return new BadRequestResult(); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }

        [Route("clients/all")]
        [HttpGet]
        public ActionResult<ClientResponse> GetAllClients()
        {
            try
            {
                IEnumerable<Client> clients;

                clients = _clientRepository.GetAll();

                if (clients.Any())
                {
                    var clientResponse = _mapper.Map<IEnumerable<ClientResponseObject>>(clients);
                    return new OkObjectResult(new ClientResponse()
                    {
                        Clients = clientResponse
                    });
                }
                else
                {
                    return new NoContentResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }

        [Route("client")]
        [HttpPost]
        public ActionResult AddClient(ClientRequestObject clientRequest)
        {
            if (clientRequest != null)
            {
                try
                {
                    var client = _mapper.Map<Client>(clientRequest);
                    _clientRepository.Add(client);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new StatusCodeResult(500);
                }

                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [Route("clients")]
        [HttpPost]
        public ActionResult AddClients([FromBody]ClientRequest clientRequest)
        {
            if (clientRequest != null && clientRequest.Clients != null && clientRequest.Clients.Any())
            {
                try
                {
                    var clients = new List<Client>();

                    foreach(var c in clientRequest.Clients.ToList())
                    {
                        var client = _mapper.Map<Client>(c);
                        clients.Add(client);
                    }
                    _clientRepository.AddRange(clients);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new StatusCodeResult(500);
                }

                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [Route("client")]
        [HttpPut]
        public ActionResult UpdateClient(int id, ClientRequestObject clientRequest)
        {
            if (clientRequest != null)
            {
                try
                {
                    var originalClient = _clientRepository.Get(id);
                    var toMerge = new Client()
                    {
                        Id = id,
                        Name = clientRequest.Name,
                        EmailAddress = clientRequest.EmailAddress,
                        RegisteredDateTime = clientRequest.RegisteredDateTime
                    };

                    var mergedClient = MergeClient(originalClient, toMerge);

                    _clientRepository.UpdateClient(mergedClient);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new StatusCodeResult(500);
                }

                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [Route("clients")]
        [HttpPut]
        public ActionResult UpdateClients([FromBody]IEnumerable<Client> clients)
        {
            if (clients != null && clients.Any())
            {
                try
                {
                    List<Client> toUpdate= new List<Client>();
                    foreach(var client in clients)
                    {
                        var originalClient = _clientRepository.Get(client.Id);
                        if(originalClient!= null)
                        {
                            var merged = MergeClient(originalClient, client);
                            toUpdate.Add(merged);
                        }
                    }

                    if(toUpdate.Any())
                    {
                        _clientRepository.UpdateClients(toUpdate);
                    }
                    else
                    {
                        return new BadRequestResult();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new StatusCodeResult(500);
                }

                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [Route("client/{id}")]
        [HttpDelete]
        public ActionResult DeleteClient(int? id)
        {
            if (id != null && id > 0)
            {
                try
                {
                    if(_clientRepository.Get(id) != null)
                    {
                        _clientRepository.Delete(id);
                        return new OkResult();
                    }
                    else
                    {
                        return new BadRequestResult();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [Route("clients")]
        [HttpDelete]
        public ActionResult DeleteClients(IEnumerable<int> ids)
        {
            if (ids != null && ids.Any())
            {
                try
                {
                    if(_clientRepository.GetRange(ids).Any())
                    {
                        _clientRepository.DeleteRange(ids);
                        return new OkResult();
                    }
                    else 
                    {
                        return new BadRequestResult();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                return new BadRequestResult();
            }
        }

        private Client MergeClient(Client originalClient, Client toMerge)
        {
            var name = !string.IsNullOrEmpty(toMerge.Name) ? toMerge.Name : originalClient.Name;

            var email = !string.IsNullOrEmpty(toMerge.EmailAddress) ?
                toMerge.EmailAddress : originalClient.EmailAddress;

            var registeredDate = toMerge.RegisteredDateTime != DateTime.MinValue ?
                toMerge.RegisteredDateTime : originalClient.RegisteredDateTime;

            var client = new Client()
            {
                Id = toMerge.Id,
                Name = name,
                EmailAddress = email,
                RegisteredDateTime = registeredDate,
            };

            return client;
        }
    }
}
