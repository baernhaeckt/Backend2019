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

- Create configuration classes for all config settings (some are used by string)
- Use IReader where possible instead of IUnitOfWork (will make it simpler to use queries later)


### Medium

- Implement EventFeed using ISubscriber
- Introduce localization

### Big

- Replace all services with commands and queries & events
- Introduce logging in the whole application

## 3. Features / Security

- Review API usage (e.g. username und pw not in query string)
- User Sign in history
- Password Reset
- Polish adding of new friends
- HealthCheck

## 4. Infrastructure

- Hosting (check for Domain & Certificate)

## 5. Testing

- Load test with 500 users

## Frontend only

- Remove prediction
