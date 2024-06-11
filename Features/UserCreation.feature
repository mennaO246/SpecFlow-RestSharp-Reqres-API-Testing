Feature: User Creation API Testing

Scenario Outline: Create user using ReqRes API
    Given user name "<name>" with job "<job>"
    When request to create user is sent
    Then user is successfully created

Examples:
| name     | job               |
| John Doe | Software Engineer |
| Sarah    | QA Engineer       |
