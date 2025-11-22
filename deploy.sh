#!/bin/bash

# Script de Deploy para FelterAPI
# Uso: ./deploy.sh [servidor]
# Exemplo: ./deploy.sh hit

set -e

SERVER=${1:-hit}
IMAGE_NAME="felterapi"
CONTAINER_NAME="felterapi"

echo "🚀 Iniciando deploy da FelterAPI para $SERVER..."

# Build da imagem Docker
echo "📦 Construindo imagem Docker..."
docker build -t $IMAGE_NAME:latest .

# Verifica se o servidor é local ou remoto
if [ "$SERVER" == "localhost" ] || [ "$SERVER" == "local" ]; then
    echo "🏠 Deploy local..."
    
    # Para o container existente se estiver rodando
    docker stop $CONTAINER_NAME 2>/dev/null || true
    docker rm $CONTAINER_NAME 2>/dev/null || true
    
    # Inicia o novo container
    docker run -d \
        --name $CONTAINER_NAME \
        -p 8080:8080 \
        --restart unless-stopped \
        $IMAGE_NAME:latest
    
    echo "✅ Deploy local concluído!"
    echo "🌐 API disponível em: http://localhost:8080"
else
    echo "🌐 Preparando para deploy remoto em $SERVER..."
    
    # Salva a imagem como tar
    echo "💾 Salvando imagem..."
    docker save $IMAGE_NAME:latest | gzip > /tmp/felterapi.tar.gz
    
    # Copia a imagem para o servidor remoto
    echo "📤 Enviando imagem para $SERVER..."
    scp /tmp/felterapi.tar.gz $SERVER:/tmp/
    
    # Faz deploy no servidor remoto
    echo "🚀 Executando deploy no servidor..."
    ssh $SERVER << 'ENDSSH'
        set -e
        IMAGE_NAME="felterapi"
        CONTAINER_NAME="felterapi"
        
        # Carrega a imagem
        echo "📥 Carregando imagem Docker..."
        docker load < /tmp/felterapi.tar.gz
        rm /tmp/felterapi.tar.gz
        
        # Para o container existente se estiver rodando
        docker stop $CONTAINER_NAME 2>/dev/null || true
        docker rm $CONTAINER_NAME 2>/dev/null || true
        
        # Inicia o novo container
        echo "▶️  Iniciando container..."
        docker run -d \
            --name $CONTAINER_NAME \
            -p 8080:8080 \
            --restart unless-stopped \
            $IMAGE_NAME:latest
        
        echo "✅ Deploy concluído no servidor!"
ENDSSH
    
    # Limpa o arquivo temporário local
    rm /tmp/felterapi.tar.gz
    
    echo "✅ Deploy remoto concluído!"
fi

echo "🎉 Deploy finalizado com sucesso!"

