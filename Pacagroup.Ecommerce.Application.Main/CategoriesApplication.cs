using Pacagroup.Ecommerce.Application.DTO;
using Pacagroup.Ecommerce.Application.Interface;
using Pacagroup.Ecommerce.Domain.Interface;
using Pacagroup.Ecommerce.Transversal.Common;
using Pacagroup.Ecommerce.Transversal.Mapper;

namespace Pacagroup.Ecommerce.Application.Main
{
    public class CategoriesApplication : ICategoriesApplication
    {
        private readonly ICategoriesDomain categoriesDomain;
        private readonly CategoriesBuilder categoriesBuilder;
        private readonly CategoriesForListBuilder categoriesForListBuilder;
        private readonly IAppLogger<CategoriesApplication> logger;

        public CategoriesApplication(ICategoriesDomain categoriesDomain, 
            CategoriesBuilder categoriesBuilder,
            CategoriesForListBuilder categoriesForListBuilder,
            IAppLogger<CategoriesApplication> logger)
        {
            this.categoriesDomain = categoriesDomain;
            this.categoriesBuilder = categoriesBuilder;
            this.categoriesForListBuilder = categoriesForListBuilder;
            this.logger = logger;
        }

        public Response<IEnumerable<CategoriesDTO>> GetAll()
        {
            Response<IEnumerable<CategoriesDTO>> response = new Response<IEnumerable<CategoriesDTO>>();
            try
            {
                var categories = categoriesDomain.GetAll();


                if (categories != null && categories.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Categorias Obtenidas exitosamente";
                    response.Data = categoriesBuilder.Convert(categories.ToList());
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se obtuvieron las Categorias";
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error en metodo GetAll. {ex.Message}", ex);

                response.IsSuccess = false;
                response.Message = $"Ocurrio un error al obtener las Categorias. {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// Listado de las categorias pero sin el campo imagen para listar en un select/bombox box
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<CategoriesListDTO>> GetForList()
        {
            Response<IEnumerable<CategoriesListDTO>> response = new Response<IEnumerable<CategoriesListDTO>>();
            try
            {
                var categories = categoriesDomain.GetAll();


                if (categories != null && categories.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "Categorias Obtenidas exitosamente";
                    response.Data = categoriesForListBuilder.Convert(categories.ToList());
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se obtuvieron las Categorias";
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error en metodo GetAll. {ex.Message}", ex);

                response.IsSuccess = false;
                response.Message = $"Ocurrio un error al obtener las Categorias. {ex.Message}";
            }

            return response;
        }
    }
}