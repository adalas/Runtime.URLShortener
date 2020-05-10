FROM node:14.2-buster AS client
EXPOSE 4200 49153

RUN mkdir /home/node/.npm-global

ENV PATH=/home/node/.npm-global/bin:$PATH
ENV NPM_CONFIG_PREFIX=/home/node/.npm-global
RUN npm install -g @angular/cli@8.1.0

WORKDIR /home/node/app
COPY src/WebApp/Runtime.URLShortener/ClientApp/package.json ./

RUN  npm  install

CMD ["ng", "serve", "--port", "4200", "--host", "0.0.0.0", "--disable-host-check", "--poll", "2000","--ssl","true","--ssl-cert","https/selfsigned.crt","--ssl-key","https/selfsigned.key"]
