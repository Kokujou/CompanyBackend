using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CompanyBackend.Models;
using Xunit;

namespace CompanyBackend.Test.IntegrationTests
{
    [Collection(nameof(CompanyBackendFixture))]
    public class CompanyControllerTest : IAsyncLifetime
    {
        private readonly HttpClient _companyClient;
        private const string RoutePrefix = "/company/";

        public static CompanyModel TestModel = new()
        {
            Id = Guid.NewGuid(),
            Name = "static Test name",
            Employees = 0,
            Equity = 0,
            LegalEntity = "static test entity"
        };

        public CompanyControllerTest(CompanyBackendFixture fixture)
        {
            _companyClient = fixture.CreateClient();
        }

        public async Task InitializeAsync()
        {
            await _companyClient.PostAsJsonAsync(RoutePrefix, TestModel);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyArray()
        {
            var response = await _companyClient.GetAsync(RoutePrefix);
            var results = await response.Content.ReadAsAsync<CompanyModel[]>();
            Assert.NotEmpty(results);
        }

        [Fact]
        public async Task Get_WhenNotExists_ReturnsNotFound()
        {
            var response = await _companyClient.GetAsync(RoutePrefix + Guid.NewGuid());
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_WhenNotExists_CreatesModel()
        {
            var model = new CompanyModel
            {
                Id = Guid.NewGuid(),
                Name = "someName",
                LegalEntity = "AG",
                Employees = 12,
                Equity = 39
            };

            var postResult = await _companyClient.PostAsJsonAsync(RoutePrefix, model);
            Assert.Equal(HttpStatusCode.Created, postResult.StatusCode);

            var response = await _companyClient.GetAsync(RoutePrefix + model.Id);
            var resultModel = await response.Content.ReadAsAsync<CompanyModel>();

            Assert.Equal(model.Id, resultModel.Id);
            Assert.Equal(model.Name, resultModel.Name);
            Assert.Equal(model.LegalEntity, resultModel.LegalEntity);
            Assert.Equal(model.Employees, resultModel.Employees);
            Assert.Equal(model.Equity, resultModel.Equity);
        }

        [Fact]
        public async Task Post_WhenExists_ReturnsBadRequest()
        {
            var result = await _companyClient.PostAsJsonAsync(RoutePrefix, TestModel);
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
        }

        [Fact]
        public async Task Get_WhenExists_ReturnsModel()
        {
            var result = await _companyClient.GetAsync(RoutePrefix + TestModel.Id);
            var model = await result.Content.ReadAsAsync<CompanyModel>();
            Assert.Equal(TestModel.Id, model.Id);
            Assert.Equal(TestModel.Name, model.Name);
            Assert.Equal(TestModel.LegalEntity, model.LegalEntity);
            Assert.Equal(TestModel.Employees, model.Employees);
            Assert.Equal(TestModel.Equity, model.Equity);
        }

        [Fact]
        public async Task Put_WhenExists_Updates()
        {
            TestModel.Name = "some other name";
            var putResult = await _companyClient.PutAsJsonAsync(RoutePrefix + TestModel.Id, TestModel);
            Assert.Equal(HttpStatusCode.NoContent, putResult.StatusCode);

            var getResult = await _companyClient.GetAsync(RoutePrefix + TestModel.Id);
            var model = await getResult.Content.ReadAsAsync<CompanyModel>();
            Assert.Equal(TestModel.Name, model.Name);
        }

        [Fact]
        public async Task Put_WhenNotExists_ReturnsNotFound()
        {
            TestModel.Id = Guid.NewGuid();
            var putResult = await _companyClient.PutAsJsonAsync(RoutePrefix + TestModel.Id, TestModel);
            Assert.Equal(HttpStatusCode.NotFound, putResult.StatusCode);
        }

        [Fact]
        public async Task Put_WhenIdMismatch_ReturnsBadRequest()
        {
            var putResult = await _companyClient.PutAsJsonAsync(RoutePrefix + Guid.NewGuid(), TestModel);
            Assert.Equal(HttpStatusCode.BadRequest, putResult.StatusCode);
        }

        [Fact]
        public async Task Delete_WhenExists_Deletes()
        {
            var deleteResult = await _companyClient.DeleteAsync(RoutePrefix + TestModel.Id);
            Assert.Equal(HttpStatusCode.NoContent, deleteResult.StatusCode);

            var getResult = await _companyClient.GetAsync(RoutePrefix + TestModel.Id);
            Assert.Equal(HttpStatusCode.NotFound, getResult.StatusCode);
        }

        [Fact]
        public async Task Delete_WhenNotExists_ReturnsNotFound()
        {
            var deleteResult = await _companyClient.DeleteAsync(RoutePrefix + Guid.NewGuid());
            Assert.Equal(HttpStatusCode.NotFound, deleteResult.StatusCode);
        }


        public async Task DisposeAsync()
        {
            await _companyClient.DeleteAsync(RoutePrefix + TestModel.Id);
        }
    }
}