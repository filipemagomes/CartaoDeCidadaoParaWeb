# Cartão do Cidadão para a Web

Este projeto foi criado de forma a poder aceder aos dados no Cartão de Cidadão a partir de qualquer página Web.

O objetivo é o preenchimento automático de formulários web com os dados(nome, NIF, morada, etc) do Cartão de Cidadão, como por exemplo: abertura de ficha de colaborador numa plataforma web de Recursos Humanos, formulário de criação de conta de loja online, etc.

### Funcionamento da aplicação

Este projeto inclui uma página muito simples para testes("index.html").

A página web faz um pedido local por http (no porto 11111) à aplicação("CartaoDeCidadaoParaWeb.Console.exe") que deve estar a correr. 

A aplicação recebe o pedido, obtém os dados do Cartão de Cidadão e devolve-os através de uma resposta em JSONP (para evitar exceções do tipo cross-domain).

A página processa os dados JSON e preenche os campos que desejarmos com os dados recebidos.

***Nota: Adaptar quer o código fonte, quer a página "index.html" de acorco com as nossas necessidades.***

***Nota: Caso a página web esteja sobre o protocolo "https" é necessário alterar o código fonte e fazer passos adicionais de forma a que a aplicação responda a pedidos "https"(não abordado aqui).***

***Nota: A título de exemplo, na página do [Portal do Cidadão](https://www.portaldocidadao.pt/login) para a obtenção os dados do Cartão de cidadão é necessário em alguns browser ter o [Java](https://www.java.com/) instalado e noutros instalar uma extensão e uma aplicação local para aceder aos dados do cartão de colaborador.***

### Pré-requisitos

```
Instalação do software do Cartão de Cidadão 
```

### Instalação

Compilar a solução e executar a aplicação "CartaoDeCidadaoParaWeb.Console.exe" diretamente a partir da pasta *bin* do projeto.

O projeto encontra-se configurado para compilar para a plataforma x64, caso queira compilar em x86, pode ser necessário ter de adicionar novamente a referência à biblioteca "pteidlib_dotnet"(instalada com o software do Cartão de Cidadão)


## Desenvolvido com

* [Software do Cartão de Cidadão](https://www.cartaodecidadao.pt/index.php_option=com_content&task=view&id=102&Itemid=44&lang=pt.html) - Middleware para aceder aos dados do cartão do cidadão
* [CSJ2K JPEG 2000 codec library ](https://www.nuget.org/packages/CSJ2K/) - Biblioteca usada para processar a fotografia que se encontra no formato jpeg2000
* [GemCard](https://github.com/orouit/SmartcardFramework/tree/master/Smartcard_API/GemCard) - Framework usada para detetar os leitores de Smart Card instalados
* [Newtonsoft.Json](https://www.nuget.org/packages/newtonsoft.json/) - Framework JSON usada para a serialização e desserialização de dados

## Versões

Para ver as versões disponíveis, consulte [as tags neste repositório](https://github.com/filipemagomes/CartaoDeCidadaoParaWeb/tags). 

## Autores

* **Filipe Gomes** [(filipemagomes)](https://github.com/filipemagomes) - *Desenvolvimento inicial*

Veja também a lista de [contributors](https://github.com/filipemagomes/CartaoDeCidadaoParaWeb/contributors)  que participaram neste projeto. 

## Licença

Este projeto é licenciado sob a Licença GNU Affero General Public License - ver o ficheiro [LICENSE.md](LICENSE.md) para mais detalhes

