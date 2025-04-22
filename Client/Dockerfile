# ----------- Build Stage -----------
FROM node:20-alpine AS build

# Set working directory
WORKDIR /app

# Install only production dependencies
COPY package*.json ./
RUN npm ci --prefer-offline --no-audit --progress=false

# Copy only necessary files for build
COPY . .

# Build the application
RUN npm run build


# ----------- Production Stage -----------
FROM nginx:alpine

# Remove default nginx config to avoid conflicts
RUN rm /etc/nginx/conf.d/default.conf

# Copy custom nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built frontend assets
COPY --from=build /app/dist /usr/share/nginx/html

# Optional: Reduce layer size (remove package manager cache)
# (not strictly needed in nginx, but good habit in custom images)
