using AutoMapper;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Transaction;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<AuthenticationResponse, SaveUserViewModel>()
               .ForMember(c => c.ConfirmPassword, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(c => c.IsVerified, opt => opt.Ignore())
               .ForMember(c => c.UserStatus, opt => opt.Ignore())
               .ForMember(c => c.Roles, opt => opt.Ignore());

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(r => r.Error, opt => opt.Ignore())
                .ForMember(r => r.HasError, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(r => r.UserStatus, opt => opt.Ignore());

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(a => a.Error , opt => opt.Ignore())
                .ForMember(a => a.HasError , opt => opt.Ignore())   
                .ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgorPasswordViewModel>()
                .ForMember(r => r.Error, opt => opt.Ignore())
                .ForMember(r => r.HasError, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(rp => rp.HasError, opt => opt.Ignore())
                .ForMember(rp => rp.Error, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(rp => rp.Token, src => src.MapFrom(x => x.Token));

            CreateMap<DtoAccounts, UserViewModel>()
                .ReverseMap();
            
            
            CreateMap<Beneficiary, BeneficiaryViewModel>()
                .ReverseMap();
            CreateMap<Beneficiary, SaveBeneficiaryViewModel>()
                .ReverseMap(); 

            CreateMap<Loan, LoanViewModel>()
               .ReverseMap();
            CreateMap<Loan, SaveLoanViewModel>()
               .ReverseMap();

            CreateMap<CreditCard, CreditCardViewModel>()
               .ReverseMap();
            CreateMap<CreditCard, SaveCreditCardViewModel>()
               .ReverseMap();

            CreateMap<SavingsAccount, SavingsAccountViewModel>()
                .ReverseMap();
            CreateMap<SavingsAccount, SaveSavingsAccountViewModel>()
               .ReverseMap();

            CreateMap<Transaction, TransactionViewModel>()
                .ReverseMap();
            CreateMap<Transaction, SaveTransactionViewModel>()
                .ReverseMap();
        }
    }
}
