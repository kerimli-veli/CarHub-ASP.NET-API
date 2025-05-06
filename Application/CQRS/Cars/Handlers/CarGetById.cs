using Application.CQRS.Cars.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Cars.Handlers;

public class CarGetById
{
    public class CarGetByIdCommand : IRequest<Result<CarGetByIdDto>>
    {
        public int Id { get; set; }
    }

    public sealed class Handler : IRequestHandler<CarGetByIdCommand, Result<CarGetByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CarGetByIdDto>> Handle(CarGetByIdCommand request, CancellationToken cancellationToken)
        {
            var currentCar = await _unitOfWork.CarRepository.GetByIdAsync(request.Id);

            if (currentCar == null)
            {
                return new Result<CarGetByIdDto>() { Errors = ["Car not found"], IsSuccess = false };
            }

            var response = _mapper.Map<CarGetByIdDto>(currentCar);

            return new Result<CarGetByIdDto>() { Data = response, Errors = [], IsSuccess = true };
        }
    }
}
