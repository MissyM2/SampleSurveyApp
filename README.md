# SampleSurveyApp

### Overview

This appcontains 14 questions
  - single selection, multi-selection and text types
  - a review page with ability to insert user selected survey data
  - generic repository

It is a .Net 7.0, .Net Maui and Xunit app that has been created as a template that includes best practices for app development.  To that end it includes the following:

  - Separation of Concerns: loosely coupling database and UI through the use of interfaces and MVVM architecture
  - DRY principles:  abstractions of database operations, use of base classes
  - Modularity that allows for unit testing the data and business logic
  - Dependency Inversion: High level modules are not dependent on low level modules.  The TodoSampleApp uses dependency injection, interfaces and abstraction.

    _Techniques_
    - MVVM architecture
    - Localization wire-up with Spanish and English
    - Shell navigation
    - CRUD services with local Sqlite db
    - Orientation detection and change of UI (Portrait and Landscape)

### Structure
Solution Name: SampleSurveyApp

Projects:
- SampleSurveyApp.Core
  Contains all data and logic-related folders, including all models, view models, services and database context for local Sqlite db.  Question and Answer data must be added to the database with required information.  A method is run at the beginning of the survey session to load all currently available questions and answers.
  - /Models
         - SurveyQuestionModel: contains questions, question type and all descriptive information for question
         - SurveyAnswerModel: contains all answers, linking field to appropriate question, navigation information for that response
         - SurveyModel: Contains information on created surveys
         - SurveyResponseModel: User selected survey responses are kept here

  - /ViewModels
         - MainPageVM
         - SurveyPageVM
         - BaseVM, Interfaces for services

  - /Services
         - Interfaces for services
  
- SampleSurveyApp.Maui
  Contains all user interface folders, including all views, images and services directly related to the UI.  These services include navigation between views, display messages and user preferences
    - /Pages
         - MainPage
         - SurveyPage
   -  /Services
         - MessageService, NavigationService, UserPreferences
         - NavigationServic
         - UserPreferences
  
 
  
- TodoSampleApp.Tests

  // Not complete:  Contains mocks of services with verification tests.  Also, contains basic test on each view model.


### Libraries

- SampleSurveyApp.Core
  
  - CommunityToolkit.Mvvm
  - sqlite-net-pcl
  - SQLitePCLRaw.bundle_green
  - SQLitePCLRaw.provider.dynamic_cdecl
 
- SampleSurveyApp.Maui

  - Microsoft.Extensions.Logging.Debug
  - CommunityToolkit.Maui
 
- SampleSurveyApp.Tests (NO TESTS, YET)

  - Microsoft.NET.Test.Sdk
  - xunit
  - xunit.runner.visualstudio
  - coverlet.collector
  - Moq
  - FluentAssertions
 
### Other Features
  - .Core
    - Task Extension
    - Validation Rules
  - .MAUI
      -   Data Triggers, Event Triggers
      -   Shell Flyout Menu with Menu footer and header
      -   QueryProperty
      -   Use of icon library and image
      -   CollectionView Grouping
      -   minimal but customized SplashScreen
      -   minimal but customized icon
