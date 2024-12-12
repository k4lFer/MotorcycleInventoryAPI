using AutoMapper;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;

namespace DataAccessLayer
{
    public static class Automapper
    {
        private static bool _initMapper = true;
        public static IMapper mapper;
        
        public static void Start()
        {
            if (_initMapper)
            {
                MapperConfiguration configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Owner, DtoOwner>().MaxDepth(3);
                    cfg.CreateMap<DtoOwner, Owner>().MaxDepth(3);
                    
                    cfg.CreateMap<User, DtoUser>().MaxDepth(3);
                    cfg.CreateMap<DtoUser, User>().MaxDepth(3);

                    cfg.CreateMap<Types, DtoTypes>().MaxDepth(3);
                    cfg.CreateMap<DtoTypes, Types>().MaxDepth(3);

                    cfg.CreateMap<Sales, DtoSales>().MaxDepth(4)
                        //.ForMember(dest => dest.ParentUser, opt => opt.MapFrom(src => src.ParentUser))
                        .ForMember(dest => dest.ChildDtoSalesDetails, opt => opt.MapFrom(src => src.ChildSalesDetails))
                        .ForMember(dest => dest.ChildDtoSalesServices, opt => opt.MapFrom(src => src.ChildSalesServices));
                    cfg.CreateMap<DtoSales, Sales>().MaxDepth(4);

                    cfg.CreateMap<Brand, DtoBrand>().MaxDepth(3);
                    cfg.CreateMap<DtoBrand, Brand>().MaxDepth(3);

                    cfg.CreateMap<Motorcycle, DtoMotorcycle>().MaxDepth(3)
                        .ForMember(dest => dest.ParentBrand, opt => opt.MapFrom(src => src.ParentBrands))
                        .ForMember(dest => dest.ParentType, opt => opt.MapFrom(src => src.ParentTypes));
                    cfg.CreateMap<DtoMotorcycle, Motorcycle>().MaxDepth(3);

                    cfg.CreateMap<Service, DtoService>().MaxDepth(3);
                    cfg.CreateMap<DtoService, Service>().MaxDepth(3);

                    cfg.CreateMap<SalesDetails, DtoSalesDetails>().MaxDepth(3)
                       //.ForMember(dest => dest.ParentDtoSales, opt => opt.MapFrom(src => src.ParentSales))
                        .ForMember(dest => dest.ParentDtoMotorcycle, opt => opt.MapFrom(src => src.ParentMotorcyle));
                    cfg.CreateMap<DtoSalesDetails, SalesDetails>().MaxDepth(3);

                    cfg.CreateMap<SalesService, DtoSalesService>().MaxDepth(3)
                        .ForMember(dest => dest.ParentDtoService, opt => opt.MapFrom(src => src.ParentService));
                        //.ForMember(dest => dest.ParentDtoSales, opt => opt.MapFrom(src => src.ParentSales));
                    cfg.CreateMap<DtoSalesService, SalesService>().MaxDepth(3);

                });
                mapper = configuration.CreateMapper();
                _initMapper = false;
            }
        }
    }
}
