using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1.Requests;

namespace TESTAPI.Validator
{
    public class CreatePostReqestValidator:AbstractValidator<CreatePostReqest>
    {
        public CreatePostReqestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$");
        }
    }
}
