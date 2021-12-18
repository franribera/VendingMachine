set -e

docker-compose -f docker-compose.yml build

docker-compose -f docker-compose.tests.yml build

docker-compose -f docker-compose.override.yml up

set +e

docker-compose -f docker-compose.test.yml run vendingmachine-api-unittests

docker-compose -f docker-compose.override.yml stop