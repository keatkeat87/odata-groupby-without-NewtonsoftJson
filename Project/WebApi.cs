using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Entity;
using Microsoft.EntityFrameworkCore;

namespace Project
{
    public class ProductsController : ODataController
    {
        private readonly CustomerOrderContext _context;

        public ProductsController(CustomerOrderContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return Ok(_context.Products);
        }
    }
}
