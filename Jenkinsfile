pipeline {
    agent any
    
    environment{
        sonar = tool name: 'sonar_scanner_dotnet'
        registry = 'shivamsahni/basicmath'
        properties = null
        docker_port = null
        username = 'shivamsahni'
        userid = 'shivam01'
        containerID = null
        containerName = 'c-shivam01-master'
        imageName = 'i-shivam01-master'
        
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
                git branch: 'master', url: 'https://github.com/shivamsahni/FinalDevOps4Dotnet.git'
            }
        }        
        stage('restore') {
            steps {
                echo "Build ${JOB_NAME} number: ${BUILD_NUMBER}"
                echo 'Restore nuget Packages'
                bat 'dotnet restore'
            }
        }
        /*stage('SonarQube Start') {
            steps {
                echo 'SonarQube Analysis'
                withSonarQubeEnv('Test_Sonar'){
                    bat "${sonar}\\SonarScanner.MSBuild.exe begin /k:sonar-shivam01 /n:sonar-shivam01 /v:1.0"
                }
            }
        }*/        
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
                bat 'dotnet test SampleDotnetWebAppTests\\SampleDotnetWebAppTests.csproj -l:trx;LogFileName=BasicMathTestResults.xml'
            }
        }
        /*stage('SonarQube Stop') {
            steps {
                echo 'Stop SonarQube Analysis'
                withSonarQubeEnv('Test_Sonar'){
                    bat "${sonar}\\SonarScanner.MSBuild.exe end"
                }
            }
        }*/        
        stage('Create Docker Image'){
            steps{
                echo "Docker Image creation step"
                bat "dotnet publish -c Release"
                bat "docker build -t ${imageName}:${BUILD_NUMBER} --no-cache -f . ."
            }
        }
        stage('Containers'){
            parallel{
                stage("Run PreContainer Checks"){
                    steps{
                        script{
                            echo "Run PreContainer Checks"
                            echo env.containerName
                            env.containerID="${bat(script: 'docker ps -q -f name=c-shivam01-master', returnStdout: true).trim()}"
                            echo "Running containerID is "
                            echo env.containerID

                            if(env.containerID!=null){
                                echo "Stop container and remove from stopped container list too"
                                bat "docker stop env.containerID && docker rm env.containerID"
                            }                         
                        }
                    }
                }
                stage("Publish Docker Image to DockerHub"){
                    steps{
                        echo "Move Image to a Docker Hub"
                        bat "docker tag ${imageName} ${registry}:${BUILD_NUMBER}"
                        withDockerRegistry([credentialsId: 'DockerHub', url: ""]){
                            bat "docker push ${registry}:${BUILD_NUMBER}"                    
                        }
                    }                    
                }
            }    
        }        
        stage('Docker Deployment'){
            steps{
                echo "Docker Deployment by using docker hub's image"
                bat "docker run --name ${containerName} ${registry}:${BUILD_NUMBER}"
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
