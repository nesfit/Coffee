# Based on official documentation:
# https://vuejs.org/v2/cookbook/dockerize-vuejs-app.html

FROM node:11-slim

RUN npm install -g http-server

ADD . /app

WORKDIR /app

RUN npm install

RUN npm run build

EXPOSE 8082

CMD [ "http-server", "dist" ]
