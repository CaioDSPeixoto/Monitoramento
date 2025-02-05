namespace MonitorDeServicos.Dominio.Helper
{
    public static class UrlHelper
    {
        public static string NormalizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("O endereço não pode ser nulo ou vazio.");
            }

            // Adiciona o protocolo http:// caso esteja ausente
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "https://" + url;
            }

            // Valida a URL
            if (!Uri.TryCreate(url, UriKind.Absolute, out var validatedUri) ||
                (validatedUri.Scheme != Uri.UriSchemeHttp && validatedUri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException("O endereço fornecido não é uma URL válida.");
            }

            return validatedUri.ToString();
        }
    }
}
