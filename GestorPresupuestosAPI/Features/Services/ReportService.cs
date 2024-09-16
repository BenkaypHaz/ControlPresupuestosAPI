using Microsoft.Reporting.WebForms;
using System.Net;
using System.Net.Http.Headers;

namespace GestorPresupuestosAPI.Features.Services
{
    public class ReportService
    {
        public async Task<byte[]> GenerateReportAsync(string reportPath, ReportParameter[] parameters = null)
        {
            // Configure the handler to use Windows authentication (NTLM)
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential("reportingservices", "ahm.123", "ahm"), // Replace with actual credentials
                PreAuthenticate = true,
                UseDefaultCredentials = false // Use this if you're providing custom credentials
            };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));

                string reportServerUrl = $"http://ahm-sql:8585/ReportServer?{reportPath}&rs:Format=PDF";

                // Append report parameters if any
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        reportServerUrl += $"&{parameter.Name}={parameter.Values[0]}";
                    }
                }

                var response = await client.GetAsync(reportServerUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    throw new Exception("Unable to generate report: " + response.ReasonPhrase);
                }
            }
        }

        public async Task<byte[]> GenerateReportExcelAsync(string reportPath, ReportParameter[] parameters = null)
        {
            // Configure the handler to use Windows authentication (NTLM)
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential("reportingservices", "ahm.123", "ahm"), // Replace with actual credentials
                PreAuthenticate = true,
                UseDefaultCredentials = false // Use this if you're providing custom credentials
            };

            using (var client = new HttpClient(handler))
            {
                // Setting up the content type to Excel
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.ms-excel"));

                // Correct format for Excel in Reporting Services
                string reportServerUrl = $"http://ahm-sql:8585/ReportServer?{reportPath}&rs:Format=EXCELOPENXML";

                // Append report parameters if any
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        reportServerUrl += $"&{parameter.Name}={parameter.Values[0]}";
                    }
                }

                var response = await client.GetAsync(reportServerUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    throw new Exception("Unable to generate report: " + response.ReasonPhrase);
                }
            }
        }



    }
}
