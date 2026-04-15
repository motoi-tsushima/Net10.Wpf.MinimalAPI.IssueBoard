using Api.MinimalAPI.IssueBoard.Data;
using Api.MinimalAPI.IssueBoard.Mapping;
using Microsoft.EntityFrameworkCore;
using Shared.Rest.IssueBoard.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddDbContext<IssuesDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseCors();

// DB が存在しない場合は自動作成
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IssuesDbContext>();
    db.Database.EnsureCreated();
}

// ── Issues エンドポイント ──────────────────────────────────────

// 一覧取得（登録日時の降順）
app.MapGet("/api/issues", async (IssuesDbContext db) =>
{
    var issues = await db.Issues
        .OrderByDescending(i => i.CreatedAt)
        .ToListAsync();
    return Results.Ok(issues.Select(i => i.ToDto()));
})
.WithName("GetIssues");

// 1件取得
app.MapGet("/api/issues/{id:int}", async (int id, IssuesDbContext db) =>
{
    var issue = await db.Issues.FindAsync(id);
    return issue is null ? Results.NotFound() : Results.Ok(issue.ToDto());
})
.WithName("GetIssue");

// 新規登録
app.MapPost("/api/issues", async (IssueCreateDto dto, IssuesDbContext db) =>
{
    var issue = dto.ToModel();
    db.Issues.Add(issue);
    await db.SaveChangesAsync();
    return Results.Created($"/api/issues/{issue.Id}", issue.ToDto());
})
.WithName("CreateIssue");

// 更新
app.MapPut("/api/issues/{id:int}", async (int id, IssueUpdateDto dto, IssuesDbContext db) =>
{
    var issue = await db.Issues.FindAsync(id);
    if (issue is null) return Results.NotFound();
    issue.UpdateModel(dto);
    await db.SaveChangesAsync();
    return Results.Ok(issue.ToDto());
})
.WithName("UpdateIssue");

// 削除
app.MapDelete("/api/issues/{id:int}", async (int id, IssuesDbContext db) =>
{
    var issue = await db.Issues.FindAsync(id);
    if (issue is null) return Results.NotFound();
    db.Issues.Remove(issue);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteIssue");

app.Run();
