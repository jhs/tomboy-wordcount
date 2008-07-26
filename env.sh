#!/bin/bash
#
# Set the pkg-config environment

if [ $(basename "$0") = 'env.sh' ]; then
    echo "This program should be sourced, not run standalone" >&2
    exit 1
fi

# Maybe it's already here.
if pkg-config --exists tomboy-addins; then
    echo "Environment is okay.  Using: `pkg-config --variable=prefix tomboy-addins`"
else
    # Otherwise, search for it.
    base='/usr/local/software'
    for app in $(find "$base" -type d -wholename '*/lib/pkgconfig' 2> /dev/null); do
        new_path="$app:$PKG_CONFIG_PATH"
        if env PKG_CONFIG_PATH="$new_path" pkg-config --exists tomboy-addins; then
            echo "Found Tomboy: $app"
            export PKG_CONFIG_PATH="$new_path"
            break
        else
            echo "no: $app"
        fi
    done
fi
