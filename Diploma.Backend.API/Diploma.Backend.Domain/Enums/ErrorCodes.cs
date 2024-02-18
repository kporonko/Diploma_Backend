using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Enums
{
    public enum ErrorCodes
    {
        InvalidPasswordException,
        UnexistingEmailException,
        ExistingEmailException,
        ApiCommunicationError,
        UserNotFound,
        InvalidUserClaims,
        TemplateNotFound,
        UnitAppearanceNotFound,
        TargetingNotFound,
        SurveyNotFound,
        SurveyUnitNotFound
    }
}
