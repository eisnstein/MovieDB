FROM nginx:1.27-alpine3.19

RUN apk --no-cache --no-progress add tini

COPY docker/nginx.conf /etc/nginx/nginx.conf
COPY docker/moviedb.conf /etc/nginx/conf.d/moviedb.conf

COPY dist/index.html /usr/share/nginx/html
COPY dist/favicon.ico /usr/share/nginx/html
COPY dist/manifest.json /usr/share/nginx/html
COPY dist/assets /usr/share/nginx/html/assets

EXPOSE 8082

ENTRYPOINT ["tini", "--", "nginx", "-g", "daemon off;"]
