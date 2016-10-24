# ms-idm-generator
Generates Model classes and unit tests by examining Identity Manager Schema

If the test, **It_generates_the_correct_property_for_a_standard_reference_attribute_that_matches_an_item_in_the_json_environment_variable**, fails, it probably means you're missing the environment variable, **CUSTOM_ATTR_TO_OBJ_MAPPINGS**, which should point to a JSON file containing object type mappings.
