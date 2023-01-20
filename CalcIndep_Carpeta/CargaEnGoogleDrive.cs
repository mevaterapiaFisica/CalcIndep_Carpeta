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


        public static void Cargar(List<object> textoAInsertar,string Equipo)
        {
            Ping p1 = new Ping();
            PingReply PR = p1.Send("drive.google.com");
            // check when the ping is not success
            if (!PR.Status.ToString().Equals("Success"))
            {
                MessageBox.Show("No se puede conectar con google drive\nReintentar en un rato");
                return;
            }
                UserCredential credential;
            // Load client secrets.
            using (var stream =
                   new FileStream(@"\\ariamevadb-svr\va_data$\Calculo Independiente\credentials.json", FileMode.Open, FileAccess.Read))
            {
                /* The file token.json stores the user's access and refresh tokens, and is created
                 automatically when the authorization flow completes for the first time. */
                string credPath = @"\\ariamevadb-svr\va_data$\Calculo Independiente\token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                                    Scopes,
                                    "user",
                                    CancellationToken.None,
                                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,

            });

            // Define request parameters.
            String spreadsheetId = "1HvxYpnQAe3eklrKRYf79mRkSb5R7ThePgOR7kglN-bE";
            String range = "Pacientes " + Equipo + "!A2:J2";
            //String range2 = "Hoja 1!F3:H3";
            var valueRange = new ValueRange();
            //List<object> lista = new List<object> {"1-123-1","Ca, Jose","2","Right","3","In","4","Up",DateTime.Today.ToShortDateString()};
            valueRange.Values = new List<IList<object>> { textoAInsertar };

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var appendResponce = appendRequest.Execute();
        }
    }
}
