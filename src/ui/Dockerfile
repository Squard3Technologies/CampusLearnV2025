# Stage 1: Build Angular app
FROM node:18-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm install --verbose
COPY . .
#RUN npm run build --configuration production
RUN npx ng build --configuration production

# Stage 2: Serve with NGINX on port 8082
FROM nginx:alpine
COPY --from=builder /app/dist/ui/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 8082
CMD ["nginx", "-g", "daemon off;"]

