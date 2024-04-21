## Projeto Redis

Este projeto foi criado com o objetivo de proporcionar uma compreensão prática sobre como utilizar o Redis como um cache de forma simples e eficaz.

### Configuração do Ambiente

Para começar, certifique-se de ter o Docker instalado em seu sistema. Em seguida, suba o ambiente utilizando o seguinte comando na raiz do projeto:

>
>docker-compose up -d

Isso inicializará tanto o Redis quanto o Redis Commander, simplificando a configuração e gestão do ambiente de desenvolvimento.

### Testando a Funcionalidade

Após a configuração do ambiente, é hora de testar a funcionalidade do projeto. Estes testes garantem a eficácia da integração com o Redis, validando e garantindo a confiabilidade das operações.

Navegue até o diretório de testes e execute o seguinte comando:

>cd test && dotnet test

<img width="765" alt="Screenshot 2024-03-30 at 20 50 19" src="https://github.com/isabeladearo/Redis/assets/92924409/0024bc72-105f-47f3-b5b1-f11ab2f521ca">

### Visualização dos Dados no Cache

Além dos testes, você pode visualizar os dados armazenados no cache acessando a seguinte URL em seu navegador: http://localhost:8081

Aqui você poderá ver os dados que estão salvos no cache em tempo real, proporcionando uma compreensão mais completa do funcionamento da aplicação.

Aproveite esta oportunidade para explorar e experimentar o Redis por si mesmo!

Até mais! 🚀

