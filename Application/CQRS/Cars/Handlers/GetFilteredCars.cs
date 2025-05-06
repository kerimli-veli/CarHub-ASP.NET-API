
using Common.GlobalResponses.Generics;
using Domain.Entities;
using Domain.Enums;
using Application.CQRS.Cars.ResponseDtos;
using MediatR;
using Repository.Common;
using AutoMapper;

namespace Application.CQRS.Cars.Handlers;

public class GetFilteredCars
{
    public class CarGetFilteredCommand : IRequest<Result<List<GetFilteredCarsAsyncDto>>>
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public FuelTypes Fuel { get; set; }
        public TransmissionTypes Transmission { get; set; }
        public double? MinMiles { get; set; }
        public double? MaxMiles { get; set; }
        public BodyTypes Body { get; set; }
        public string? Color { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CarGetFilteredCommand, Result<List<GetFilteredCarsAsyncDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<GetFilteredCarsAsyncDto>>> Handle(CarGetFilteredCommand request, CancellationToken cancellationToken)
        {
            var filter = new CarFilterModel
            {
                Brand = request.Brand,
                Model = request.Model,
                MinYear = request.MinYear,
                MaxYear = request.MaxYear,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                Fuel = request.Fuel,
                Transmission = request.Transmission,
                MinMiles = request.MinMiles,
                MaxMiles = request.MaxMiles,
                Body = request.Body,
                Color = request.Color
            };

            var filteredCars = await _unitOfWork.CarRepository.GetFilteredCarsAsync(filter);


            var response = _mapper.Map<List<GetFilteredCarsAsyncDto>>(filteredCars);

            return new Result<List<GetFilteredCarsAsyncDto>>()
            {
                Data = response,
                IsSuccess = true,
                Errors = []
            };
        }
    }
}

