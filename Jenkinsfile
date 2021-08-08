pipeline {
    agent any
    
    environment{
        sonar = tool name: 'sonar_scanner_dotnet'
        registry = 'shivamsahni/basicmath'
        properties = null
        docker_port = null
        username = 'shivamsahni'
        userid = 'shivam01'
        containerName = 'c-shivam01-master'
        imageName = 'i-shivam01-master'
        project_id = 'shivamnagp'
        cluster_name = 'shivam01-cluster'
        location = 'us-central1-c'
        credentialsId = 'GKE_Shivam01'       
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
        stage('SonarQube Start') {
            steps {
                echo 'SonarQube Analysis'
                withSonarQubeEnv('Test_Sonar'){
                    bat "${sonar}\\SonarScanner.MSBuild.exe begin /k:sonar-shivam01 /n:sonar-shivam01 /v:1.0"
                }
            }
        }               
        stage('build') {
            steps {
                echo 'Clean before build'
                bat 'dotnet clean'
                echo 'Build Code'
                bat 'dotnet build'
            }
        } 
        stage('SonarQube Stop') {
            steps {
                echo 'Stop SonarQube Analysis'
                withSonarQubeEnv('Test_Sonar'){
                    bat "${sonar}\\SonarScanner.MSBuild.exe end"
                }
            }
        }       
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
                    environment{
                        containerID = "${bat(script: 'docker ps -a -q -f name="c-shivam01-master"', returnStdout: true).trim().readLines().drop(1).join("")}"
                    }
                    steps{
                        script{
                            echo "Run PreContainer Checks"
                            echo env.containerName
                            echo "containerID is "
                            echo env.containerID
                            
                            if(env.containerID != null){
                                echo "Stop container and remove from stopped container list too"
                                bat "docker stop ${env.containerID} && docker rm ${env.containerID}"
                            }                         
                        }
                    }
                }
                stage("Publish Docker Image to DockerHub"){
                    steps{
                        echo "Move Image to a Docker Hub"
                        bat "docker tag ${imageName}:latest ${registry}:latest"                        
                        bat "docker tag ${imageName}:${BUILD_NUMBER} ${registry}:${BUILD_NUMBER}"
                        withDockerRegistry([credentialsId: 'DockerHub', url: ""]){
                            bat "docker push ${registry}:latest"
                            bat "docker push ${registry}:${BUILD_NUMBER}"
                        }       
                    }                    
                }
            }    
        }        
        stage('Docker Deployment'){
            steps{
                echo "Docker Deployment by using docker hub's image"
                bat "docker run -d -p 7200:80 --name ${containerName} ${registry}:${BUILD_NUMBER}"
            }
        }
        stage('Kubernetes Deployment'){
            steps{
                echo "Kubernetes Deployment"
                bat "kubectl apply -f deployment.yaml"
            }
        }        
    }
}
