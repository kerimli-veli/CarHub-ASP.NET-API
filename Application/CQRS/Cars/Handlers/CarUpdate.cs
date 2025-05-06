using Application.CQRS.Cars.ResponseDtos;
using Application.Services;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Repository.Common;

namespace Application.CQRS.Cars.Handlers;

public class CarUpdate
{
    public class UpdateCarCommand : IRequest<Result<CarUpdateDto>>
    {
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public FuelTypes Fuel { get; set; }
        public TransmissionTypes Transmission { get; set; }
        public double Miles { get; set; }

        public IFormFile? MainImage { get; set; }
        public IFormFile? FirstSideImage { get; set; }
        public IFormFile? SecondSideImage { get; set; }
        public IFormFile? EngineImage { get; set; }
        public IFormFile? SalonImage { get; set; }

        public BodyTypes Body { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public string Text { get; set; }
    }


    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateCarCommand, Result<CarUpdateDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CarUpdateDto>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            var currentCar = await _unitOfWork.CarRepository.GetByIdAsync(request.CarId);
            if (currentCar == null)
                throw new BadRequestException($"Car does not exist with id {request.CarId}");

            currentCar.Brand = request.Brand;
            currentCar.Model = request.Model;
            currentCar.Year = request.Year;
            currentCar.Price = request.Price;
            currentCar.Fuel = request.Fuel;
            currentCar.Transmission = request.Transmission;
            currentCar.Miles = request.Miles;
            currentCar.Body = request.Body;
            currentCar.Color = request.Color;
            currentCar.VIN = request.VIN;
            currentCar.Text = request.Text;
            currentCar.UpdatedBy = currentCar.UserId;

            var carImage = currentCar.CarImagePaths.FirstOrDefault();
            if (carImage == null)
            {
                carImage = new CarImage();
                currentCar.CarImagePaths.Add(carImage);
            }

            // Hər bir şəkil üçün ayrıca yoxlayırıq
            if (request.MainImage != null)
            {
                var mainImagePath = await ImageService.SaveImageAsync(request.MainImage, "uploads/cars");
                carImage.MainImage = mainImagePath;
            }

            if (request.FirstSideImage != null)
            {
                var firstSideImagePath = await ImageService.SaveImageAsync(request.FirstSideImage, "uploads/cars");
                carImage.FirstSideImage = firstSideImagePath;
            }

            if (request.SecondSideImage != null)
            {
                var secondSideImagePath = await ImageService.SaveImageAsync(request.SecondSideImage, "uploads/cars");
                carImage.SecondSideImage = secondSideImagePath;
            }

            if (request.EngineImage != null)
            {
                var engineImagePath = await ImageService.SaveImageAsync(request.EngineImage, "uploads/cars");
                carImage.EngineImage = engineImagePath;
            }

            if (request.SalonImage != null)
            {
                var salonImagePath = await ImageService.SaveImageAsync(request.SalonImage, "uploads/cars");
                carImage.SalonImage = salonImagePath;
            }

            _unitOfWork.CarRepository.Update(currentCar);

            var carUpdateDto = new CarUpdateDto
            {
                Brand = currentCar.Brand,
                Model = currentCar.Model,
                Year = currentCar.Year,
                Price = currentCar.Price,
                Fuel = currentCar.Fuel,
                Transmission = currentCar.Transmission,
                Miles = currentCar.Miles,
                Body = currentCar.Body,
                Color = currentCar.Color,
                VIN = currentCar.VIN,
                Text = currentCar.Text,
                CarImagePaths = currentCar.CarImagePaths
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

            return new Result<CarUpdateDto>
            {
                Data = carUpdateDto,
                Errors = [],
                IsSuccess = true
            };
        }
    }

}
