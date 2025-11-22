#!/bin/bash

# Script de Deploy para FelterAPI no servidor hit
# Deploy via dotnet publish + SSH

set -e

SERVER="hit"
REMOTE_PATH="/app/felterapi"
REMOTE_USER=${2:-root}

echo "🚀 Iniciando deploy da FelterAPI para $SERVER..."

# Limpa builds anteriores
echo "🧹 Limpando builds anteriores..."
rm -rf bin/Release
rm -rf publish

# Publica a aplicação
echo "📦 Publicando aplicação .NET..."
dotnet publish -c Release -o publish

# Copia arquivos para o servidor
echo "📤 Copiando arquivos para $SERVER..."
scp -r publish/* $REMOTE_USER@$SERVER:$REMOTE_PATH/

# Reinicia o serviço no servidor remoto
echo "🔄 Reiniciando serviço no servidor..."
ssh $REMOTE_USER@$SERVER << 'ENDSSH'
    set -e
    REMOTE_PATH="/app/felterapi"
    
    # Para o serviço se estiver rodando
    systemctl stop felterapi 2>/dev/null || true
    
    # Reinicia o serviço
    systemctl start felterapi || {
        echo "⚠️  Serviço systemd não encontrado. Tentando executar diretamente..."
        cd $REMOTE_PATH
        dotnet FelterAPI.dll &
    }
    
    echo "✅ Serviço reiniciado com sucesso!"
ENDSSH

echo "✅ Deploy concluído com sucesso!"
echo "🎉 FelterAPI está rodando no servidor $SERVER!"

