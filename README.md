# LEAF

TODO: Insert description here.

## Getting started

TODO: Describe how to start the application and how to run tests, to generate test data.

## Architecture

TODO: document architecture here.

## Coding Guidelines

### Logging

- Do log the start and end of a command/query/eventhandler (by default use the information-level, if indicated otherwise, use the trace-level) .
- Do not log exceptions/errors twice.
- Do use semantic logging.
- Do follow the guideline below for common log statements
- Do not log for (all those exceptions are logged by a middleware):
	- Exceptions which can not be handled
	- ValidationExceptions
	- EntityNotFoundExceptions

For Commands:
- Execute ...: Execute xy
- Execute ...Successful: Executed xy

For Queries:
- Retrieve...: Retrieve ...
- Retrieve...Successful: Retrieved ...

For EventHandlers:
- Handle...: Handle xyEvent: Do xy zz.
- Handle...Successful: Handled xyEvent.

Remark: Manually logging is enforced to ensure that no sensitiv data is logged.