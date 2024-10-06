﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.UseCases.Interfaces
{
    public interface IUseCase<TRequest, TResponse>
    {
        Task<TResponse> Invoke(TRequest request);
    }
}
