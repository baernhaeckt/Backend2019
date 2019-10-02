# TODO

## 1. High test coverage

### int tests

- Backend.Core.Features.Ranking.Controllers.RankingsController
- Backend.Core.Features.Quiz.Controllers.QuizController
- Backend.Core.Features.PointsAndAwards.Controllers.PointsController
- Backend.Core.Features.Newsfeed.Hubs.NewsfeedHub
- Backend.Core.Features.Baseline.Controllers.SufficientTypeController
- Backend.Core.Features.Newsfeed.Controllers.EventTestController

### Unit Tests

- Backend.Core.Extensions.ListExtensions
- Backend.Core.Extensions.PrincipalExtensions

## 2. Refactoring

### Small

- Use IReader where possible instead of IUnitOfWork (will make it simpler to use queries later)
- Move partners to database (currently hardcoded in TokenService.cs)

### Medium

- Implement EventFeed using ISubscriber
- Introduce localization
- Make sure that the integration tests can run in memory as well
- For token generation, use id from currently logged in partner but enable to generate a specific kind, of a token

### Big

- Replace all services with commands and queries & events
- Introduce logging in the whole application

## 3. Features

- Token which can be used without a limit
- Complete profile update functionality + location
- Password rest
- Polish adding of new friends
- HealthCheck

## 4. Infrastructure

- Implement staging
- Hosting (check for Domain & Certificate)

## 5. Testing

- Load test with 500 users

## Frontend only

- Remove prediction
