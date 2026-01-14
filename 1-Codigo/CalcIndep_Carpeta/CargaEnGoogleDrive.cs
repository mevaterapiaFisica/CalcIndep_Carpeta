using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalcIndep_Carpeta
{
    public static class CargaEnGoogleDrive
    {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "UploadPatMove";


        public static void SubirADriveOGuardarLocal(List<object> textoAInsertar, string Equipo)
        {
            if(EsEntornoCitrix())
            {
                GuardarEnArchivoLocal(textoAInsertar, Equipo);
            }
            else if (TieneAccesoAGoogle())
            {
                //MessageBox.Show("Tiene acceso a google");
                SubirADrive(textoAInsertar, Equipo);
            }
            else
            {
                GuardarEnArchivoLocal(textoAInsertar, Equipo);
                //MessageBox.Show("No tiene acceso a google");
            }
        }

        public static bool EsEntornoCitrix()
        {
            // 1. Verificar la variable SESSIONNAME
            string sessionName = Environment.GetEnvironmentVariable("SESSIONNAME");
            if (!string.IsNullOrEmpty(sessionName) && sessionName.ToUpper().StartsWith("ICA"))
            {
                return true;
            }

            // 2. Verificar si existe la variable CLIENTNAME (solo existe en sesiones remotas)
            string clientName = Environment.GetEnvironmentVariable("CLIENTNAME");
            if (!string.IsNullOrEmpty(clientName))
            {
                return true;
            }

            return false;
        }

        public static bool TieneAccesoAGoogle()
        {
            try
            {
                // Intentamos resolver el dominio de Google Sheets
                // Si no hay internet/DNS, esto lanzará una excepción inmediatamente
                IPAddress[] addresslist = Dns.GetHostAddresses("sheets.googleapis.com");

                if (addresslist.Length > 0)
                {
                    //MessageBox.Show("AddressList > 0");
                    // Opcional: Intentar una conexión rápida por el puerto 443 (HTTPS)
                    using (var client = new TcpClient())
                    {
                        var result = client.BeginConnect("sheets.googleapis.com", 443, null, null);
                        bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2)); // Timeout de 2 seg
                        /*if (success)
                        {
                            MessageBox.Show("Conexion success");
                        }*/
                        return success;
                    }
                }
                return false;
            }
            catch
            {
                // Si no pudo resolver el nombre o no hubo conexión, devolvemos false
                return false;
            }
        }
        private static void GuardarEnArchivoLocal(List<object> textoAInsertar, string equipo)
        {
            // Generar un nombre único para cada envío
            string usuario = Environment.UserName;
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            string fileName = $"corrimientos_{equipo}_{usuario}_{timestamp}.csv";

            string fullPath = Path.Combine(@"\\ARIAMEVADB-SVR\va_data$\Calculo Independiente\Corrimientos", fileName);

            // Guardar el archivo (se crea, se escribe y se cierra inmediatamente)
            File.WriteAllText(fullPath, string.Join(";",textoAInsertar.Select(o => o?.ToString() ?? string.Empty).ToArray()));
            MessageBox.Show("No se pudo conectar con RefToIso.\nEl archivo se guardó de forma local");
        }

        public static void SubirADrive(List<object> textoAInsertar, string Equipo)
        {
            // Verifica la conectividad
            /*Ping p1 = new Ping();
            PingReply PR = p1.Send("drive.google.com");
            if (!PR.Status.ToString().Equals("Success"))
            {
                MessageBox.Show("No se puede conectar con Google Drive\nReintentar en un rato");
                return;
            }*/

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
            else if (Equipo.Contains("Q_"))
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
