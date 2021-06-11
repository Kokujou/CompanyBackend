using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyBackend.Models;
using CompanyBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace CompanyBackend.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(ICompanyRepository repository)
        {
            _companyRepository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> GetAllAsync()
        {
            try
            {
                return Ok(await _companyRepository.GetCompaniesAsync());
            }
            catch (CosmosException e)
            {
                return StatusCode((int) e.StatusCode, e.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CompanyModel>> GetAsync(Guid id)
        {
            try
            {
                return Ok(await _companyRepository.GetCompanyAsync(id));
            }
            catch (CosmosException e)
            {
                return StatusCode((int) e.StatusCode, e.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] CompanyModel model)
        {
            try
            {
                await _companyRepository.CreateCompanyAsync(model);
                return CreatedAtAction("Get", new {id = model.Id}, model);
            }
            catch (CosmosException e)
            {
                return StatusCode((int) e.StatusCode, e.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateAsync([FromBody] CompanyModel model, Guid id)
        {
            try
            {
                if (id != model.Id) return BadRequest("the ids of the objects do not match!");
                var test = await _companyRepository.GetCompanyAsync(id);
                await _companyRepository.UpdateCompanyAsync(model);
                return NoContent();
            }
            catch (CosmosException e)
            {
                return StatusCode((int) e.StatusCode, e.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _companyRepository.DeleteCompanyAsync(id);
                return NoContent();
            }
            catch (CosmosException e)
            {
                return StatusCode((int) e.StatusCode, e.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}