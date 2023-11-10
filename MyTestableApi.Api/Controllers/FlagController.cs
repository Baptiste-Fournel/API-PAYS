using Microsoft.AspNetCore.Mvc;
using System.Text;


namespace MyTestableApi
{
    [ApiController]
    [Route("[controller]")]
    public class FlagController : ControllerBase
    {
        private readonly ILogger<FlagController> _logger;

        public FlagController(ILogger<FlagController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "GetFlag")]
        public async Task<IActionResult> GetFlagAsync([FromBody] string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
            {
                return BadRequest("Le nom du pays est invalide.");
            }

            // Chemin complet vers le fichier CSV
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../pays.csv");

            try
            {
                using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filePath, Encoding.UTF8))
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        // Vérifie si le nom du pays correspond
                        if (fields != null && fields.Length >= 2 && fields[0] == countryName)
                        {
                            var flag = new Flag
                            {
                                Country = fields[0],
                                FlagLink = fields[1]
                            };

                            // Vérifie si le lien du drapeau est vide
                            if (string.IsNullOrWhiteSpace(flag.FlagLink))
                            {
                                return NoContent();
                            }

                            return Ok(flag);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la lecture du fichier CSV : {ex.Message}");
                return StatusCode(500, "Erreur interne du serveur");
            }

            return NotFound($"Aucun drapeau trouvé pour le pays : {countryName}");
        }
    }
    public class Flag
    {
        public string Country { get; set; }
        public string FlagLink { get; set; }
    }
}