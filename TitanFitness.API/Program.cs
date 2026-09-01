using Microsoft.EntityFrameworkCore;
using TitanFitness.Infrastructure.Persistence;
using TitanFitness.Application.Interfaces;
using TitanFitness.Infrastructure.Repositories;
using FluentValidation;
using TitanFitness.Application.Plans.Commands.CreatePlan;
using TitanFitness.Application.Plans.Queries.GetPlanByID;
using TitanFitness.Application.Plans.Queries.GetPlans;
using TitanFitness.Application.Plans.Commands.UpdatePlan;
using TitanFitness.Application.Trainers.Commands.CreateTrainer;
using TitanFitness.Application.Trainers.Queries.GetTrainers;
using TitanFitness.Application.Trainers.Queries.GetTrainerById;
using TitanFitness.Application.Trainers.Commands.UpdateTrainer;
using TitanFitness.Application.Members.Commands.CreateMember;
using TitanFitness.Application.Members.Commands.UpdateMember;
using TitanFitness.Application.Members.Queries.GetMembers;
using TitanFitness.Application.Members.Queries.GetMemberById;
using TitanFitness.Application.Branches.Commands.CreateBranch;
using TitanFitness.Application.Branches.Queries.GetBranches;
using TitanFitness.Application.Studios.Commands.CreateStudio;
using TitanFitness.Application.Studios.Queries.GetStudiosByBranch;
using TitanFitness.Application.Memberships.Commands.PurchaseMembership;
using TitanFitness.Application.Memberships.Queries.GetMembershipsByMember;
using TitanFitness.Application.Freezes.Commands.CreateFreeze;
using TitanFitness.Application.CheckIns.Commands.CreateCheckIn;
using TitanFitness.Application.Sessions.Commands.CreateSession;
using TitanFitness.Application.Sessions.Queries.GetSessions;
using TitanFitness.Application.Bookings.Commands.CreateBooking;
using TitanFitness.Application.Bookings.Commands.CancelBooking;
using TitanFitness.Application.Memberships.Commands.RenewMembership;
using TitanFitness.Application.Memberships.Commands.CancelMembership;
using TitanFitness.Application.GuestPasses.Commands.IssueGuestPass;
using TitanFitness.Application.GuestPasses.Commands.UseGuestPass;
using TitanFitness.Application.GuestPasses.Queries.GetGuestPasses;
using TitanFitness.Application.Dashboard.Queries.GetDashboard;
using TitanFitness.Application.Bookings.Queries.GetBookingsBySession;
using TitanFitness.Application.Bookings.Queries.GetBookingsByMember;
using TitanFitness.Application.CheckIns.Queries.GetCheckInsByMember;
using TitanFitness.Application.Memberships.Commands.ChangeMembershipPlan;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// connects the api to sql server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// repository and unit of work
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// finds validators inside the application project
builder.Services.AddValidatorsFromAssemblyContaining<CreatePlanCommandValidator>();

// adds the create plan handler
builder.Services.AddScoped<CreatePlanCommandHandler>();

// adds the get plan handler
builder.Services.AddScoped<GetPlansQueryHandler>();

// adds the get plan by id handler
builder.Services.AddScoped<GetPlanByIdQueryHandler>();

// adds updateplan handler
builder.Services.AddScoped<UpdatePlanCommandHandler>();

// adds creating for trainer handler
builder.Services.AddScoped<CreateTrainerCommandHandler>();

// adds getting for trainer handler
builder.Services.AddScoped<GetTrainersQueryHandler>();

// adds getting by ID for trainer handler
builder.Services.AddScoped<GetTrainerByIdQueryHandler>();

// adds put for trainer handler
builder.Services.AddScoped<UpdateTrainerCommandHandler>();

builder.Services.AddScoped<CreateMemberCommandHandler>();
builder.Services.AddScoped<UpdateMemberCommandHandler>();
builder.Services.AddScoped<GetMembersQueryHandler>();
builder.Services.AddScoped<GetMemberByIdQueryHandler>();

builder.Services.AddScoped<CreateBranchCommandHandler>();
builder.Services.AddScoped<GetBranchesQueryHandler>();
builder.Services.AddScoped<CreateStudioCommandHandler>();
builder.Services.AddScoped<GetStudiosByBranchQueryHandler>();

builder.Services.AddScoped<PurchaseMembershipCommandHandler>();
builder.Services.AddScoped<GetMembershipsByMemberQueryHandler>();

builder.Services.AddScoped<CreateFreezeCommandHandler>();
builder.Services.AddScoped<CreateCheckInCommandHandler>();

builder.Services.AddScoped<CreateSessionCommandHandler>();
builder.Services.AddScoped<GetSessionsQueryHandler>();
builder.Services.AddScoped<CreateBookingCommandHandler>();
builder.Services.AddScoped<CancelBookingCommandHandler>();

builder.Services.AddScoped<RenewMembershipCommandHandler>();
builder.Services.AddScoped<CancelMembershipCommandHandler>();
builder.Services.AddScoped<IssueGuestPassCommandHandler>();
builder.Services.AddScoped<UseGuestPassCommandHandler>();
builder.Services.AddScoped<GetGuestPassesQueryHandler>();

builder.Services.AddScoped<GetDashboardQueryHandler>();
builder.Services.AddScoped<GetBookingsBySessionQueryHandler>();
builder.Services.AddScoped<GetBookingsByMemberQueryHandler>();
builder.Services.AddScoped<GetCheckInsByMemberQueryHandler>();

builder.Services.AddScoped<ChangeMembershipPlanCommandHandler>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
