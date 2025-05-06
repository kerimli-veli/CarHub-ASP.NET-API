using AutoMapper;
using Domain.Entities;
using static Application.CQRS.Categories.Handlers.AddCategory;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using static Application.CQRS.Products.Handlers.AddProduct;
using Application.CQRS.Categories.ResponseDtos;
using Application.CQRS.Products.ResponsesDto;

﻿using Application.CQRS.Users.ResponseDtos;
using static Application.CQRS.Users.Handlers.Register;
using static Application.CQRS.Users.Handlers.Update;

using Application.CQRS.Cars.ResponseDtos;
using Application.CQRS.Users.Handlers;
using Application.CQRS.Cart.ResponseDtos;
using static Application.CQRS.Cart.Handlers.AddCart;
using static Application.CQRS.Cart.Handlers.AddProductToCart;
using Application.CQRS.Carts.Handlers;
using static Application.CQRS.Carts.Handlers.GetCartWithLinesByUserId;
using static Application.CQRS.Cars.Handlers.CarAdd;
using static Application.CQRS.Cart.Handlers.GetCartWithLines;
using static Application.CQRS.Cart.Handlers.UpdateProductQuantityInCart;
using Application.CQRS.Cart.Queries;
using static Application.CQRS.Cart.Queries.GetCartTotalPrice;
using static Application.CQRS.Products.Handlers.GetByNameProduct;
using Application.CQRS.SignalR.ResponseDtos;
using Microsoft.AspNetCore.Http;
using Application.CQRS.Order.ResponseDtos;
using static CreateOrder;
using Application.CQRS.Auctions.ResponseDtos;
using Application.CQRS.Notifications.ResponseDtos;


namespace Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Notifications Mapping
        CreateMap<Notification, GetAllNotificationsDto>();

        //Auctions Mapping
        CreateMap<Auction, AuctionResponseDto>();
        CreateMap<Auction, GetAllActiveAsyncDto>();
        CreateMap<Auction, AuctionActivatedNotificationDto>();

        
        //Category Mapping
        CreateMap<Category, CategoryAddDto>();
        CreateMap<AddCommand, Category>();

        CreateMap<Category, CategoryGetAllDto>();
        CreateMap<CategoryGetAllDto, Category>();

        CreateMap<Category, UpdateDto>();
        CreateMap<UpdateDto, Category>();

        CreateMap<Category, DeleteDto>();
        CreateMap<DeleteDto, Category>();

        CreateMap<GetCategoriesWithProductsDto, Category>();
        CreateMap<Category, GetCategoriesWithProductsDto>();
        CreateMap<Product, ProductDto>();


        //Product Mapping
        CreateMap<Product, AddProductDto>();
        CreateMap<AddProductCommand, Product>();

        //CreateMap<Product, GetAllProductDto>().ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath));
        CreateMap<Product, GetAllProductDto>();
        CreateMap<GetAllProductDto, Product>();

        CreateMap<Product, UpdateProductDto>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<Product, GetByNameProductDto>();
        CreateMap<ProductGetByNameQuery, Product>();

        CreateMap<Product , ProductResponseDto>();
        CreateMap<ProductResponseDto, Product>();

        CreateMap<Product, GetProductsByPriceRange>();

        //User Mapping
        CreateMap<RegisterCommand, User>().ReverseMap();
        CreateMap<User, RegisterDto>();

        CreateMap<User, UpdateDto>();
        CreateMap<User, UserGetAllDto>();
        CreateMap<User, GetByIdDto>();
        CreateMap<Car, GetUserFavoritesDto>();
        CreateMap<Car, GetUserCarsDto>();
        CreateMap<Car, GetFilteredCarsAsyncDto>();

        //Car Mapping
        
        CreateMap<Car, CarGetAllDto>();
        CreateMap<Car, CarGetByIdDto>();

        //Cart
        
        CreateMap<AddCartCommand, Cart>();
        CreateMap<Cart, AddCartDto>();

        CreateMap<AddProductToCartCommand, Cart > ();
        CreateMap<Cart, AddProductToCartDto>();

        CreateMap<Cart, GetCartWithLinesByUserIdDto>().ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Cart, GetCartWithLinesDto>();

        CreateMap<Cart, UpdateProductQuantityInCartDto>();
        CreateMap<UpdateProductQuantityInCartCommand ,Cart>();

        CreateMap<Cart, GetTotalPriceDto>();


        //SignalR

        CreateMap<ChatMessage, ChatMessageDto>();
        CreateMap<ChatMessage, ChatMessageDto>()
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
            .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.SentAt));


        //Order
        CreateMap<Order, CreateOrderDto>();
        CreateMap<CreateOrderCommand, Order>();

        CreateMap<Order, OrderDto>();

    }
}
