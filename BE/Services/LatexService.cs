using System.Diagnostics;

namespace BE.Services
{
    public class LatexService
    {
        public async Task<(byte[] pdfData, string outputMessage)> ConvertLatextoPdfAsync(string latexCode)
        {
            string latexFilePath = Path.Combine(Path.GetTempPath(), "document.tex");
            string pdfFilePath = Path.ChangeExtension(latexFilePath, ".pdf");
            //string logFilePath = Path.ChangeExtension(latexFilePath, ".log");

            await System.IO.File.WriteAllTextAsync(latexFilePath, latexCode);

            // process config
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "pdflatex",
                Arguments = $" -interaction=nonstopmode -output-directory {Path.GetDirectoryName(latexFilePath)} {latexFilePath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    return (null, output);
                }

                byte[] pdfData = await System.IO.File.ReadAllBytesAsync(pdfFilePath);

                System.IO.File.Delete(latexFilePath);
                System.IO.File.Delete(pdfFilePath);
                //System.IO.File.Delete(logFilePath);

                return (pdfData, null);
            }
        }

    }
}
