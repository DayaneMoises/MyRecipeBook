﻿using AutoMapper;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.Services.AutoMapper;

//funciona como config do automapper, vai preparar o mapeamento entre requisição e uma entidade
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User >()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }

    private void DomainToRequest()
    {

    }
}
