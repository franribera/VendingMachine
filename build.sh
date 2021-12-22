set -e

docker-compose -f docker-compose.yml build

docker-compose -f docker-compose.tests.yml build

docker-compose -f docker-compose.override.yml up -d vending-database

sleep 10

docker-compose -f docker-compose.override.yml up -d

set +e

docker-compose -f docker-compose.tests.yml run --rm vendingmachine-api-unittests

docker-compose -f docker-compose.tests.yml run --rm vendingmachine-api-integrationtests

docker-compose -f docker-compose.override.yml stop