using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace CalcIndep_Carpeta
{
    public static class CargaEnGoogleDrive
    {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "UploadPatMove";


        public static void Cargar(List<object> textoAInsertar, string Equipo)
        {
            // Verifica la conectividad
            Ping p1 = new Ping();
            PingReply PR = p1.Send("drive.google.com");
            if (!PR.Status.ToString().Equals("Success"))
            {
                MessageBox.Show("No se puede conectar con Google Drive\nReintentar en un rato");
                return;
            }

            UserCredential credential;

            // Define las rutas base
            string basePath = @"\\ariamevadb-svr\va_data$\Calculo Independiente\";
            string credFileName = "credentials.json";
            string tokenFileName = "token.json";

            // Si Equipo contiene "Q_", cambia las rutas
            if (Equipo.Contains("Q_"))
            {
                basePath = @"\\ariamevadb-svr\va_data$\Calculo Independiente Quilmes\";
            }

            // Construye las rutas completas
            string credentialsFile = Path.Combine(basePath, credFileName);
            string tokenPath = Path.Combine(basePath, tokenFileName);

            // Carga las credenciales
            using (var stream = new FileStream(credentialsFile, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenPath, true)).Result;
            }

            // Crea el servicio de Google Sheets API
            var service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define el ID de la hoja de cálculo según el Equipo
            string spreadsheetId = "";
            if (Equipo == "Medrano" || Equipo == "Equipo 2")
            {
                spreadsheetId = "1HvxYpnQAe3eklrKRYf79mRkSb5R7ThePgOR7kglN-bE";
            }
            else if (Equipo == "Q_Equipo1" || Equipo == "Q_Equipo2")
            {
                if (Equipo == "Q_Equipo1")
                {
                    Equipo = "Equipo 1";
                }
                else
                {
                    Equipo = "Equipo 2";
                }
                spreadsheetId = "1Z6dTImEfdJjvmungbXOPxYyRJZL0huVGaEfeoelVVoY";
            }

            // Resto del código para enviar los datos a Google Sheets
            string range = "Pacientes " + Equipo + "!A2:J2";
            var valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>> { textoAInsertar };

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var appendResponse = appendRequest.Execute();
        }

    }
}
