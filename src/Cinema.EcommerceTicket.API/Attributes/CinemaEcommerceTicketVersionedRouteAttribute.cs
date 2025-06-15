using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.API.Attributes;

[ExcludeFromCodeCoverage]
public class CinemaEcommerceTicketVersionedRouteAttribute(string version) : RouteAttribute($"api/v{version}/[controller]") { }
