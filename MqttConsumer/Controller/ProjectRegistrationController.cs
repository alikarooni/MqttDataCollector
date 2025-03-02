using Microsoft.AspNetCore.Mvc;
using MqttConsumer.Repositories;
using MqttConsumer.Entites;
using MqttConsumer.Entities;
using System;

namespace MqttConsumer;

[Route("api/[controller]")]
public class ProjectRegistrationController : ControllerBase
{
    private readonly IDomainRepository _domainRepository;
    private readonly IProjectRepository _projectRepository;

    public ProjectRegistrationController(IDomainRepository domainRepository, IProjectRepository projectRepository)
    {
        _domainRepository = domainRepository;
        _projectRepository = projectRepository;
    }


    [HttpPost]
    [Route("GetDomains")]
    public IActionResult GetDomains()
    {
        return Ok(new { Domains = _domainRepository.GetDomains() });
    }

    [HttpPost]
    [Route("GetProjects")]
    public IActionResult GetProjects()
    {
        return Ok(new { Projects = _projectRepository.GetProjects() });
    }

    [HttpPost]
    [Route("AddDomain")]
    public IActionResult AddDomain([FromBody] Domain domain)
    {
        _domainRepository.AddDomain(
            domain.DomainName, domain.DomainEndPoint, domain.DomainSecretKey);

        return new OkObjectResult("We are cool!");
    }

    [HttpPost]
    [Route("AddProject")]
    public IActionResult AddProject([FromBody] Project project)
    {
        _projectRepository.AddProject(
            project.DomainId, project.ProjectName, project.BrokerUrl, project.Username,
            project.Password, project.Port, project.UseTls, project.BrokerTopic, project.EventGridTopic);

        return new OkObjectResult("We are cool!");
    }
}