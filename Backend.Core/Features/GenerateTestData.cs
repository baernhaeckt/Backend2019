using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Entities.Partner;
using Backend.Core.Features.Partner.Commands;
using Backend.Core.Features.Quiz.Commands;
using Backend.Core.Features.Quiz.Data;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Persistence;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features
{
    public class GenerateTestData : IStartupTask
    {
        private const int tokensPerUser = 10;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ICommandPublisher _commandPublisher;

        public GenerateTestData(IUnitOfWork unitOfWork, ICommandPublisher commandPublisher)
        {
            _unitOfWork = unitOfWork;
            _commandPublisher = commandPublisher;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            List<User> allUsers = (await _unitOfWork.GetAllAsync<User>()).ToList();

            var tokens = new Stack<Guid>();

            List<TokenIssuer> tokenIssuers = (await _unitOfWork.GetAllAsync<TokenIssuer>()).ToList();
            while (tokens.Count < allUsers.Count * tokensPerUser)
            {
                foreach (TokenIssuer tokenIssuer in tokenIssuers)
                {
                    var createNewTokenCommand = new CreateNewTokenCommand(tokenIssuer.Id, tokenIssuer.PrototypeTokens.First().TokenType);
                    tokens.Push(await _commandPublisher.ExecuteAsync(createNewTokenCommand));
                }
            }

            Parallel.ForEach(allUsers, async user =>
            {
                for (int i = 0; i < tokensPerUser; i++)
                {
                    var rewardUserTokenCommand = new RewardUserTokenCommand(tokens.Pop(), user.Id);
                    await _commandPublisher.ExecuteAsync(rewardUserTokenCommand);
                }

                // Every user answers 4 questions. 3 are correct, one is wrong.
                var answerQuizQuestionCommand1 = new AnswerQuizQuestionCommand(user.Id, GenerateQuizQuestionsStartupTask.Question1.Id, GenerateQuizQuestionsStartupTask.Question1.Answers.First(q => q.IsCorrect).Id);
                var answerQuizQuestionCommand2 = new AnswerQuizQuestionCommand(user.Id, GenerateQuizQuestionsStartupTask.Question2.Id, GenerateQuizQuestionsStartupTask.Question2.Answers.First(q => q.IsCorrect).Id);
                var answerQuizQuestionCommand3 = new AnswerQuizQuestionCommand(user.Id, GenerateQuizQuestionsStartupTask.Question3.Id, GenerateQuizQuestionsStartupTask.Question3.Answers.First(q => !q.IsCorrect).Id);
                var answerQuizQuestionCommand4 = new AnswerQuizQuestionCommand(user.Id, GenerateQuizQuestionsStartupTask.Question4.Id, GenerateQuizQuestionsStartupTask.Question4.Answers.First(q => q.IsCorrect).Id);
                await _commandPublisher.ExecuteAsync(answerQuizQuestionCommand1);
                await _commandPublisher.ExecuteAsync(answerQuizQuestionCommand2);
                await _commandPublisher.ExecuteAsync(answerQuizQuestionCommand3);
                await _commandPublisher.ExecuteAsync(answerQuizQuestionCommand4);
            });
        }
    }
}