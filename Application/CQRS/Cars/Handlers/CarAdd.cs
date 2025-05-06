using Application.CQRS.Cars.ResponseDtos;
using Application.Services;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Repository.Common;
using System.Linq;

namespace Application.CQRS.Cars.Handlers;

public class CarAdd
{
    public class CarAddCommand : IRequest<Result<CarAddDto>>
    {
        public int UserId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public FuelTypes Fuel { get; set; }
        public TransmissionTypes Transmission { get; set; }
        public double Miles { get; set; }
        public List<IFormFile> CarImagePaths { get; set; }
        public BodyTypes Body { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public string Text { get; set; }
    }

    public sealed class Handler : IRequestHandler<CarAddCommand, Result<CarAddDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CarAddDto>> Handle(CarAddCommand request, CancellationToken cancellationToken)
        {
            var car = new Car
            {
                UserId = request.UserId,
                Brand = request.Brand,
                Model = request.Model,
                Year = request.Year,
                Price = request.Price,
                Fuel = request.Fuel,
                Transmission = request.Transmission,
                Miles = request.Miles,
                Body = request.Body,
                Color = request.Color,
                VIN = request.VIN,
                Text = request.Text,
                CreatedBy = request.UserId,
                CarImagePaths = new List<CarImage>()
            };

            if (request.CarImagePaths != null && request.CarImagePaths.Any())
            {
                var images = new List<string>();

                foreach (var image in request.CarImagePaths)
                {
                    var imagePath = await ImageService.SaveImageAsync(image, "uploads/cars");
                    images.Add(imagePath);
                }

                var carImage = new CarImage
                {
                    MainImage = images.ElementAtOrDefault(0),
                    FirstSideImage = images.ElementAtOrDefault(1),
                    SecondSideImage = images.ElementAtOrDefault(2),
                    EngineImage = images.ElementAtOrDefault(3),
                    SalonImage = images.ElementAtOrDefault(4)
                };

                car.CarImagePaths.Add(carImage);
            }

            await _unitOfWork.CarRepository.AddAsync(car);

            var carAddDto = new CarAddDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Price = car.Price,
                Fuel = car.Fuel,
                Transmission = car.Transmission,
                Miles = car.Miles,
                Body = car.Body,
                Color = car.Color,
                VIN = car.VIN,
                Text = car.Text,
                CarImagePaths = car.CarImagePaths
                    .SelectMany(ci => new List<string>
                    {
                        ci.MainImage,
                        ci.FirstSideImage,
                        ci.SecondSideImage,
                        ci.EngineImage,
                        ci.SalonImage
                    })
                    .Where(path => !string.IsNullOrEmpty(path))
                    .ToList()
            };

            return new Result<CarAddDto>
            {
                Data = carAddDto,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}
