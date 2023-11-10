namespace MyTestableApi.Tests
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;
    using System.Net.Http;

    public class UnitTest1
    {
        /*
         * GIVEN je récupère le drapeau d'un pays existant
         * WHEN je fais un appel POST sur /Flag avec le nom du pays
         * THEN je reçois un json représentant le drapeau du pays
         * AND je reçois un code 200
         */
        [Fact]
        public async Task IsGetFlagOk()
        {
            // Arrange
            await using var _factory = new WebApplicationFactory<Program>();
            var client = _factory.CreateClient();
            var countryName = "France";

            // Act
            var response = await client.PostAsync("Flag", new StringContent($"\"{countryName}\"", null, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(await response.Content.ReadAsStringAsync());
            // Ajoutez des assertions supplémentaires pour vérifier le contenu du drapeau si nécessaire
        }

        /*
         * GIVEN je récupère le drapeau d'un pays inexistant
         * WHEN je fais un appel POST sur /Flag avec le nom du pays
         * THEN je reçois un code 404
         */
        [Fact]
        public async Task IsGetFlagNotFound()
        {
            // Arrange
            await using var _factory = new WebApplicationFactory<Program>();
            var client = _factory.CreateClient();
            var countryName = "PaysInconnu";

            // Act
            var response = await client.PostAsync("Flag", new StringContent($"\"{countryName}\"", null, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}