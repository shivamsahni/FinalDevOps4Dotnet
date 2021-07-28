pipeline {
    agent any
    
    environment{
        sonar = tool name: 'sonar_scanner_dotnet'
        registry = 'shivamsahni/basicmath'
        properties = null
        docker_port = null
        username = 'shivamsahni'
        userid = 'shivam01'
    }
    
    options {
      timestamps()
      timeout(activity: true, time: 1, unit: 'HOURS')
      skipDefaultCheckout()
      buildDiscarder(logRotator(
          numToKeepStr:'3',
          daysToKeepStr:'5'
      ))      
    }
    
    stages {
        stage('git checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/shivamsahni/FinalDevOps4Dotnet.git
            }
        }        
        stage('restore') {
            steps {
                echo "Build ${JOB_NAME} number: ${BUILD_NUMBER}"
                echo 'Restore nuget Packages'
                bat 'dotnet restore'
            }
        }
        stage('clean') {
            steps {
                echo 'Clean before build'
                bat 'dotnet clean'
            }
        }
        stage('build') {
            steps {
                echo 'Build Code'
                bat 'dotnet build'
            }
        } 
        stage('Automated Unit Testing') {
            steps {
                echo 'Run Unit Tests'
                bat 'dotnet test BasicMathTests\\BasicMathTests.csproj -l:trx;LogFileName=BasicMathTestResults.xml'
            }
        }
        stage('Create Docker Image'){
            steps{
                echo "Docker Image creation step"
                bat "dotnet publish -c Release"
                bat "docker build -t i-${userid}-master --no-cache -f . ."
            }
        }		
    }
    post{
        always{
            echo 'Test Report Generation...'
            xunit([MSTest(deleteOutputFiles: true, failIfNotNew: true, pattern: 'BasicMathTests\\TestResults\\BasicMathTestResults.xml', skipNoTestFiles: true, stopProcessingIfError: true)])
        }
        
    }
}