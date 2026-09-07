
namespace ConsultoriosApi.Api.Utils
{

    public static class HttpContextExtentions
    {
        public static void InsertPagingInHeader(this HttpContext httpContext, int RecordsTotal)
        {
            httpContext.Response.Headers.Append("records-total-quantity", RecordsTotal.ToString());
        }
    }
}