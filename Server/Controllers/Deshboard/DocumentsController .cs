
using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EskomAdmin.Server.Models.Deshboard;
using EskomAdmin.Server.Data;

namespace EskomAdmin.Server.Controllers.Deshboard;

[Route("upload/multiple")]
[ApiController]
public class DocumentsController : ControllerBase
{
private readonly DeshboardContext _context;

public DocumentsController(DeshboardContext context)
{
    _context = context;
}

[HttpGet("{id}")]
public IActionResult GetPdf(int id)
{
    var doc = _context.Deshboards.FirstOrDefault(d => d.id == id);
    if (doc == null) return NotFound();

    return File( doc.FileUpload ?? "application/pdf", $"{doc.FileUpload}.pdf");
}
  
}