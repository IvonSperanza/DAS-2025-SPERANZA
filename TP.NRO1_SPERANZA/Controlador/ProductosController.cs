namespace Controlador
{

    public class ProductosController : Controller
    {
        private ProductoData data = new ProductoData();

        public ActionResult Index()
        {
            try
            {
                var productos = data.ObtenerTodos();
                return View(productos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

       
        public ActionResult Create(Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (data.Insertar(producto))
                    {
                        TempData["Mensaje"] = "Producto creado exitosamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "No se pudo crear el producto";
                    }
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(producto);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var producto = data.ObtenerPorId(id);
                if (producto == null)
                {
                    return HttpNotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (data.Actualizar(producto))
                    {
                        TempData["Mensaje"] = "Producto actualizado exitosamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "No se pudo actualizar el producto";
                    }
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(producto);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var producto = data.ObtenerPorId(id);
                if (producto == null)
                {
                    return HttpNotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
