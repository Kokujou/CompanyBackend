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
        public async Task<ActionResult<IEnumerable<CompanyModel>>> GetAll()
        {
            try
            {
                return Ok(await _companyRepository.GetCompanies());
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
                return Ok(await _companyRepository.GetCompany(id));
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
                await _companyRepository.CreateCompany(model);
                return Ok();
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

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] CompanyModel model)
        {
            try
            {
                await _companyRepository.UpdateCompany(model);
                return Ok();
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
                await _companyRepository.DeleteCompany(id);
                return Ok();
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